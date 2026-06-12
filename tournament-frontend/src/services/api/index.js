import { realApi } from "./realApi";
import { mockApi } from "./mockApi";

let api = null;

/**
 * Try backend once
 */
async function detectApi() {
  try {
    const res = await fetch("https://localhost:7275/api/tournaments");
    if (res.ok) {
      console.log("🟢 REAL API ACTIVE");
      return realApi;
    }
  } catch (e) { }

  console.log("🟡 MOCK API ACTIVE");
  return mockApi;
}

/**
 * Promise that resolves once
 */
export const apiServicePromise = (async () => {
  api = await detectApi();
  return api;
})();