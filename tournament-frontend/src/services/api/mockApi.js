let tournaments = [
  {
    id: 1,
    title: "Demo Tournament",
    description: "Mock tournament",
    maxPlayers: 8,
    date: new Date(Date.now() + 86400000).toISOString()
  }
];

let games = [
  {
    id: 1,
    title: "Mock Game",
    tournamentId: 1,
    time: new Date(Date.now() + 86400000).toISOString()
  }
];

const delay = (ms) => new Promise(r => setTimeout(r, ms));

const id = () => Date.now();

/* ---------------- HELPERS ---------------- */

function validateTournament(dto) {
  if (!dto.title || dto.title.length < 3) {
    throw new Error("Title must be at least 3 characters");
  }

  if (!dto.description) {
    throw new Error("Description is required");
  }

  if (!dto.maxPlayers || dto.maxPlayers < 1) {
    throw new Error("MaxPlayers must be at least 1");
  }

  if (!dto.date || new Date(dto.date) <= new Date()) {
    throw new Error("Date must be in the future");
  }
}

/* ---------------- API ---------------- */

export const mockApi = {
  /* ---------------- TOURNAMENTS ---------------- */

  async getTournaments() {
    await delay(100);
    return [...tournaments];
  },

  async createTournament(dto) {
    await delay(100);

    validateTournament(dto);

    const newTournament = {
      id: id(),
      title: dto.title,
      description: dto.description,
      maxPlayers: dto.maxPlayers,
      date: dto.date
    };

    tournaments.push(newTournament);
    return newTournament;
  },

  async updateTournament(idToUpdate, dto) {
    await delay(100);

    validateTournament(dto);

    tournaments = tournaments.map(t =>
      t.id === idToUpdate ? { ...t, ...dto } : t
    );

    return tournaments.find(t => t.id === idToUpdate);
  },

  async deleteTournament(idToDelete) {
    await delay(100);

    tournaments = tournaments.filter(t => t.id !== idToDelete);
    games = games.filter(g => g.tournamentId !== idToDelete);

    return { success: true };
  },

  /* ---------------- GAMES ---------------- */

  async getGames() {
    await delay(100);
    return [...games];
  },

  async createGame(dto) {
    await delay(100);

    if (!dto.title || dto.title.length < 3) {
      throw new Error("Title must be at least 3 characters");
    }

    const newGame = {
      id: id(),
      title: dto.title,
      tournamentId: dto.tournamentId,
      time: dto.time
    };

    games.push(newGame);
    return newGame;
  },

  async deleteGame(idToDelete) {
    await delay(100);

    games = games.filter(g => g.id !== idToDelete);
    return { success: true };
  }
};