let tournaments = [
  { id: 1, title: "Demo Tournament" }
];

let games = [
  { id: 1, title: "Grand Tournament 2026", tournamentId: 1, time: new Date().toISOString() }
];

export const db = {
  tournaments,
  games
};