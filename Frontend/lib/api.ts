const API_BASE =
  process.env.NEXT_PUBLIC_API_URL ||
  "https://safaapi-exbqbkacgkgjb9dq.canadacentral-01.azurewebsites.net"

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
  getLeads: () => fetcher("/api/v1/leads"),

  // ⭐ AI
  getNextActions: (leadId: number) =>
    fetcher(`/recommend/next-actions/${leadId}`),

  draftColdEmail: (leadId: number) =>
    fetcher(`/draft/cold-email/${leadId}`, { method: "POST" }),

  draftLinkedIn: (leadId: number) =>
    fetcher("/draft/linkedin", {
      method: "POST",
      body: JSON.stringify({ leadId }),
    }),

  draftFollowUp: (leadId: number) =>
    fetcher("/draft/follow-up", {
      method: "POST",
      body: JSON.stringify({ leadId }),
    }),
}