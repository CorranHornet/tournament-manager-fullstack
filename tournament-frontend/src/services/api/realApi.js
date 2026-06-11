const BASE_URL = "https://localhost:7275/api";

/* ---------------- helper ---------------- */

async function handle(res) {
  if (!res.ok) {
    const err = await res.json().catch(() => ({}));
    throw err;
  }
  return res.json();
}

/* ---------------- TOURNAMENTS ---------------- */

async function getTournaments() {
  return handle(await fetch(`${BASE_URL}/tournaments`));
}

async function createTournament(dto) {
  return handle(await fetch(`${BASE_URL}/tournaments`, {
    method: "POST",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify(dto)
  }));
}

async function updateTournament(id, dto) {
  return handle(await fetch(`${BASE_URL}/tournaments/${id}`, {
    method: "PUT",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify(dto)
  }));
}

async function deleteTournament(id) {
  return handle(await fetch(`${BASE_URL}/tournaments/${id}`, {
    method: "DELETE"
  }));
}

/* ---------------- GAMES ---------------- */

async function getGames() {
  return handle(await fetch(`${BASE_URL}/games`));
}

async function createGame(dto) {
  return handle(await fetch(`${BASE_URL}/games`, {
    method: "POST",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify(dto)
  }));
}

async function deleteGame(id) {
  return handle(await fetch(`${BASE_URL}/games/${id}`, {
    method: "DELETE"
  }));
}

export const realApi = {
  getTournaments,
  createTournament,
  updateTournament,
  deleteTournament,
  getGames,
  createGame,
  deleteGame
};