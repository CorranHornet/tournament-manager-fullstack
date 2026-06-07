// CHANGE THIS PORT to match backend 
const BASE_URL = 'https://localhost:7275/api';

export const apiService = {
  // GET all games
  async getGames() {
    const response = await fetch(`${BASE_URL}/games`);
    if (!response.ok) throw new Error('Failed to fetch games');
    return response.json();
  },

  // GET all tournaments (Added for dropdown functionality)
  async getTournaments() {
    const response = await fetch(`${BASE_URL}/tournaments`);
    if (!response.ok) throw new Error('Failed to fetch tournaments');
    return response.json();
  },

  // POST a new game
  async createGame(gameData) {
    const response = await fetch(`${BASE_URL}/games`, {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify(gameData)
    });
    if (!response.ok) throw new Error('Failed to create game');
    return response.json();
  },

  // PUT (Update) a game
  async updateGame(id, gameData) {
    const response = await fetch(`${BASE_URL}/games/${id}`, {
      method: 'PUT',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify(gameData)
    });
    if (!response.ok) throw new Error('Failed to update game');
    return response;
  },

  // DELETE a game
  async deleteGame(id) {
    const response = await fetch(`${BASE_URL}/games/${id}`, {
      method: 'DELETE'
    });
    if (!response.ok) throw new Error('Failed to delete game');
    return response;
  }
};