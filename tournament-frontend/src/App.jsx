import { useState, useEffect } from 'react';
import { apiService } from './services/apiService';
import './App.css';

function App() {
  const [games, setGames] = useState([]);
  const [tournaments, setTournaments] = useState([]); // State to hold tournament list
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  // Form and navigation states
  const [currentTab, setCurrentTab] = useState('view');
  const [title, setTitle] = useState('');
  const [tournamentId, setTournamentId] = useState('');
  const [gameDate, setGameDate] = useState('');
  const [editingGameId, setEditingGameId] = useState(null);

  useEffect(() => {
    loadData();
  }, []);

  // Fetch both games and tournaments on initialization
  const loadData = async () => {
    try {
      setLoading(true);
      const [gamesData, tournamentsData] = await Promise.all([
        apiService.getGames(),
        apiService.getTournaments() // Ensure this function exists in apiService
      ]);
      setGames(gamesData);
      setTournaments(tournamentsData);
      setError(null);
    } catch (err) {
      setError('Could not connect to the API. Check backend status.');
    } finally {
      setLoading(false);
    }
  };

  const getMinDate = () => {
    return new Date().toISOString().split('T')[0];
  };

  const handleSubmit = async (e) => {
    e.preventDefault();

    const gamePayload = {
      title,
      tournamentId: parseInt(tournamentId),
      time: new Date(gameDate).toISOString()
    };

    try {
      if (editingGameId) {
        await apiService.updateGame(editingGameId, gamePayload);
        setEditingGameId(null);
      } else {
        await apiService.createGame(gamePayload);
      }

      setTitle('');
      setTournamentId('');
      setGameDate('');
      setCurrentTab('view');
      await loadData(); // Reload all data
    } catch (err) {
      console.error(err);
      setError('Failed to save. Ensure date is in the future and data is valid.');
    }
  };

  return (
    <div className="app-container">
      <header>
        <h1>🏆 Tournament Manager</h1>
      </header>

      <nav className="nav-tabs">
        <button
          className={`tab-btn ${currentTab === 'view' ? 'active' : ''}`}
          onClick={() => setCurrentTab('view')}>📋 View Games</button>
        <button
          className={`tab-btn ${currentTab === 'create' ? 'active' : ''}`}
          onClick={() => setCurrentTab('create')}>➕ Add/Edit Game</button>
      </nav>

      {error && <div className="error-box">⚠️ {error}</div>}

      {currentTab === 'view' ? (
        <div>
          {games.map(game => (
            <div key={game.id} className="game-card">
              <div>
                <h3>{game.title}</h3>
                <small>Date: {new Date(game.time).toLocaleDateString()}</small>
              </div>
              <div>
                <button className="btn btn-edit" onClick={() => {
                  setEditingGameId(game.id);
                  setTitle(game.title);
                  setTournamentId(game.tournamentId);
                  setCurrentTab('create');
                }}>Edit</button>
                <button className="btn btn-delete" onClick={() => apiService.deleteGame(game.id).then(loadData)}>Delete</button>
              </div>
            </div>
          ))}
        </div>
      ) : (
        <section className="form-section">
          <form onSubmit={handleSubmit}>
            <label>Game Title</label>
            <input
              value={title}
              onChange={(e) => setTitle(e.target.value)}
              required
              minLength={3}
            />

            <label>Tournament</label>
            {/* VG improvement: Use select dropdown to prevent invalid IDs */}
            <select
              value={tournamentId}
              onChange={(e) => setTournamentId(e.target.value)}
              required
            >
              <option value="">-- Select a Tournament --</option>
              {tournaments.map(t => (
                <option key={t.id} value={t.id}>{t.title}</option>
              ))}
            </select>

            <label>Tournament Date</label>
            <input
              type="date"
              value={gameDate}
              onChange={(e) => setGameDate(e.target.value)}
              min={getMinDate()}
              required
            />

            <button className="btn btn-primary" type="submit">Save Game</button>
          </form>
        </section>
      )}
    </div>
  );
}

export default App;