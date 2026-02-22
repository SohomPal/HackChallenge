const API_BASE =
  process.env.NEXT_PUBLIC_API_URL ||
  "https://safaapi-exbqbkacgkgjb9dq.canadacentral-01.azurewebsites.net/api/v1"

async function fetcher<T>(path: string, options?: RequestInit): Promise<T> {
  const res = await fetch(`${API_BASE}${path}`, {
    headers: {
      "Content-Type": "application/json",
    },
    cache: "no-store",
    ...options,
  })

  if (!res.ok) {
    const text = await res.text()
    throw new Error(text || "API error")
  }

  return res.json()
}

export const api = {
  // ⭐ CORE
  getLeads: () => fetcher("/leads"),

  // ⭐ AI
  getNextActions: (leadId: number) =>
    fetcher(`/ai/recommend/next-actions/${leadId}`),

  draftColdEmail: (leadId: number) =>
    fetcher(`/ai/draft/cold-email/${leadId}`, { method: "GET" }),

  draftLinkedIn: (leadId: number) =>
    fetcher("/ai/draft/linkedin", {
      method: "POST",
      body: JSON.stringify({ leadId }),
    }),

  draftFollowUp: (leadId: number) =>
    fetcher("/ai/draft/follow-up", {
      method: "POST",
      body: JSON.stringify({ leadId }),
    }),
}