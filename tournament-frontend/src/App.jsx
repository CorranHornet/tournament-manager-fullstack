import { useEffect, useState } from "react";
import { apiServicePromise } from "./services/api";
import "./App.css";

export default function App() {
  const [apiService, setApiService] = useState(null);

  const [games, setGames] = useState([]);
  const [tournaments, setTournaments] = useState([]);

  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  const [title, setTitle] = useState("");
  const [tournamentId, setTournamentId] = useState("");
  const [gameDate, setGameDate] = useState("");

  const [newTournamentTitle, setNewTournamentTitle] = useState("");

  /* ---------------- INIT API ---------------- */

  useEffect(() => {
    async function init() {
      const service = await apiServicePromise;
      setApiService(service);
    }

    init();
  }, []);

  /* ---------------- LOAD DATA ---------------- */

  useEffect(() => {
    if (!apiService) return;
    loadData();
  }, [apiService]);

  async function loadData() {
    try {
      setLoading(true);

      const [gamesData, tournamentsData] = await Promise.all([
        apiService.getGames(),
        apiService.getTournaments()
      ]);

      setGames(gamesData);
      setTournaments(tournamentsData);
      setError(null);
    } catch (err) {
      console.error(err);
      setError("Failed to load data");
    } finally {
      setLoading(false);
    }
  }

  /* ---------------- GUARDS ---------------- */

  if (!apiService) {
    return <div className="app-container">Initializing API...</div>;
  }

  if (loading) {
    return <div className="app-container">Loading...</div>;
  }

  /* ---------------- CREATE GAME ---------------- */

  async function handleCreateGame(e) {
    e.preventDefault();

    try {
      await apiService.createGame({
        title,
        tournamentId: parseInt(tournamentId),
        time: new Date(gameDate).toISOString()
      });

      setTitle("");
      setTournamentId("");
      setGameDate("");

      await loadData();
    } catch (err) {
      console.error(err);
      setError("Failed to create game");
    }
  }

  /* ---------------- CREATE TOURNAMENT ---------------- */

  async function handleCreateTournament() {
    try {
      await apiService.createTournament({
        title: newTournamentTitle,
        description: "Default tournament",
        maxPlayers: 8,
        date: new Date(Date.now() + 86400000).toISOString()
      });

      setNewTournamentTitle("");
      await loadData();
    } catch (err) {
      console.error("TOURNAMENT ERROR:", err);

      // 👇 extract backend validation message if it exists
      const message =
        err?.errors?.Title?.[0] ||
        err?.message ||
        "Failed to create tournament";

      setError(message);
    }
  }

  /* ---------------- UI ---------------- */

  return (
    <div className="app-container">
      <h1>🏆 Tournament Manager</h1>

      {error && <div className="error-box">{error}</div>}

      <section className="form-section">
        <h3>Create Tournament</h3>

        <input
          value={newTournamentTitle}
          onChange={(e) => setNewTournamentTitle(e.target.value)}
          placeholder="Tournament name"
        />

        <button onClick={handleCreateTournament}>
          Add Tournament
        </button>

        {/* ================= TOURNAMENT LIST ================= */}

        <div style={{ marginTop: "20px" }}>
          {tournaments.length === 0 ? (
            <p>No tournaments available</p>
          ) : (
            tournaments.map(t => (
              <div key={t.id} className="game-card">
                <div>
                  <h4>{t.title}</h4>
                  <small>{t.description}</small>
                </div>

                <button
                  className="btn btn-delete"
                  onClick={async () => {
                    try {
                      await apiService.deleteTournament(t.id);
                      await loadData();
                    } catch (err) {
                      setError("Failed to delete tournament");
                    }
                  }}
                >
                  Delete
                </button>
              </div>
            ))
          )}
        </div>
      </section>
      {/* CREATE GAME */}
      <form className="form-section" onSubmit={handleCreateGame}>
        <h3>Create Game</h3>

        <input
          value={title}
          onChange={(e) => setTitle(e.target.value)}
          placeholder="Game title"
          minLength={3}
          required
        />

        <select
          value={tournamentId}
          onChange={(e) => setTournamentId(e.target.value)}
          required
        >
          <option value="">Select tournament</option>
          {tournaments.map(t => (
            <option key={t.id} value={t.id}>
              {t.title}
            </option>
          ))}
        </select>

        <input
          type="date"
          value={gameDate}
          onChange={(e) => setGameDate(e.target.value)}
          required
        />

        <button type="submit">Create Game</button>
      </form>

      {/* LIST */}
      <div>
        {games.map(g => (
          <div key={g.id} className="game-card">
            <div>
              <h3>{g.title}</h3>
              <small>{new Date(g.time).toLocaleDateString()}</small>
            </div>

            <button onClick={() => apiService.deleteGame(g.id).then(loadData)}>
              Delete
            </button>
          </div>
        ))}
      </div>
    </div>
  );
}