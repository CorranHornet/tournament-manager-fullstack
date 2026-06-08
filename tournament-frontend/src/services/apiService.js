// BASE_URL for local development
const BASE_URL = 'https://localhost:7275/api';

// Fallback data to ensure the UI works even without a backend connection
const MOCK_GAMES = [
  { id: 1, title: "Grand Tournament 2026", tournamentId: 1 },
  { id: 2, title: "Summer Championship", tournamentId: 1 }
];

export const apiService = {
  // GET all games
  async getGames() {
    try {
      const response = await fetch(`${BASE_URL}/games`);
      if (!response.ok) throw new Error('Failed to fetch games');
      return await response.json();
    } catch (error) {
      console.warn("Backend unreachable, using mock data for demo purposes.");
      return MOCK_GAMES;
    }
  },

  // GET all tournaments
  async getTournaments() {
    try {
      const response = await fetch(`${BASE_URL}/tournaments`);
      if (!response.ok) throw new Error('Failed to fetch tournaments');
      return await response.json();
    } catch (error) {
      console.warn("Backend unreachable, using mock tournaments.");
      return [{ id: 1, name: "Demo Tournament" }];
    }
  },

  // POST a new game
  async createGame(gameData) {
    try {
      const response = await fetch(`${BASE_URL}/games`, {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(gameData)
      });
      if (!response.ok) throw new Error('Failed to create game');
      return await response.json();
    } catch (error) {
      console.error("Cannot create game: Backend is offline.");
      return { ...gameData, id: Date.now() }; // Simulate success locally
    }
  },

  // PUT (Update) a game
  async updateGame(id, gameData) {
    try {
      const response = await fetch(`${BASE_URL}/games/${id}`, {
        method: 'PUT',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(gameData)
      });
      if (!response.ok) throw new Error('Failed to update game');
      return response;
    } catch (error) {
      console.error("Cannot update game: Backend is offline.");
      return { status: 200 }; // Simulate success locally
    }
  },

  // DELETE a game
  async deleteGame(id) {
    try {
      const response = await fetch(`${BASE_URL}/games/${id}`, {
        method: 'DELETE'
      });
      if (!response.ok) throw new Error('Failed to delete game');
      return response;
    } catch (error) {
      console.error("Cannot delete game: Backend is offline.");
      return { status: 200 }; // Simulate success locally
    }
  }
};