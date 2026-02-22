// src/app/page.tsx
"use client"

import { useState, useMemo, useEffect } from "react"
import { TopBar } from "@/components/top-bar"
import { PipelineBoard } from "@/components/pipeline-board"
import { AIPriorities } from "@/components/ai-priorities"
import { LeadDrawer } from "@/components/lead-drawer"
import { TaskCenter } from "@/components/task-center"
import { api } from "@/lib/api"

// keep types from mock-data since UI already uses them
import type { Lead, Task, AutomationRule } from "@/lib/mock-data"
import { tasks as mockTasks, automationRules as mockRules } from "@/lib/mock-data"

import type { AppView } from "@/components/top-bar"

export default function DashboardPage() {
  const [searchQuery, setSearchQuery] = useState("")
  const [selectedLead, setSelectedLead] = useState<Lead | null>(null)
  const [drawerOpen, setDrawerOpen] = useState(false)
  const [activeView, setActiveView] = useState<AppView>("pipeline")

  // ⭐ backend state
  const [leads, setLeads] = useState<Lead[]>([])
  const [priorities, setPriorities] = useState<Lead[]>([])
  const [loading, setLoading] = useState(true)
  const [error, setError] = useState<string | null>(null)

  // ⭐ fallback state (until backend endpoints exist)
  const [tasks] = useState<Task[]>(mockTasks)
  const [automationRules] = useState<AutomationRule[]>(mockRules)

  /**
   * STEP 1 — load leads
   */
  useEffect(() => {
    async function loadLeads() {
      try {
        setLoading(true)
        setError(null)

        const data = await api.getLeads()

        /**
         * Adapt backend lead → UI Lead (from mock-data)
         * Backend has: id, name, company, email, phone, linkedIn, fitScore
         * UI expects: stage, score, owner, lastContact, aiReasons, etc.
         */
        const mapped: Lead[] = data.map((l: any) => ({
          // IMPORTANT: keep id type aligned with your UI Lead type.
          // If your UI Lead.id is a string, use String(l.id).
          id: String(l.id),

          name: l.name ?? "",
          company: l.company ?? "",

          // map FitScore -> score
          score: typeof l.fitScore === "number" ? l.fitScore : 0,

          // fields not in DB → safe defaults
          stage: "new",
          lastContact: "",
          owner: "",

          // AI fields used by AIPriorities
          aiReasons: [],
        }))

        setLeads(mapped)
      } catch (e: any) {
        console.error(e)
        setError("Failed to load leads")
      } finally {
        setLoading(false)
      }
    }

    loadLeads()
  }, [])

  /**
   * STEP 2 — load AI priorities
   * (you don't have AI reasons yet, so we just pick top 5 by score)
   */
  useEffect(() => {
    if (!leads.length) return

    async function loadPriorities() {
      try {
        // example: top 5 by score
        const subset = [...leads].sort((a, b) => (b.score ?? 0) - (a.score ?? 0)).slice(0, 5)

        // optional: warm next-action endpoint if it exists; ignore failures
        await Promise.all(subset.map((l) => api.getNextActions(Number(l.id)).catch(() => null)))

        setPriorities(subset)
      } catch (e) {
        console.error(e)
        setPriorities(leads.slice(0, 5))
      }
    }

    loadPriorities()
  }, [leads])

  /**
   * Search filter
   */
  const filteredLeads = useMemo(() => {
    if (!searchQuery.trim()) return leads
    const q = searchQuery.toLowerCase()

    return leads.filter(
      (lead) =>
        (lead.name ?? "").toLowerCase().includes(q) ||
        (lead.company ?? "").toLowerCase().includes(q)
    )
  }, [searchQuery, leads])

  /**
   * UI states
   */
  if (loading) {
    return (
      <div className="flex h-screen items-center justify-center text-sm text-muted-foreground">
        Loading dashboard…
      </div>
    )
  }

  if (error) {
    return (
      <div className="flex h-screen items-center justify-center text-sm text-red-500">
        {error}
      </div>
    )
  }

  return (
    <div className="flex h-screen flex-col">
      <TopBar
        searchQuery={searchQuery}
        onSearchChange={setSearchQuery}
        activeView={activeView}
        onViewChange={setActiveView}
      />

      <div className="flex flex-1 overflow-hidden">
        {activeView === "pipeline" && (
          <>
            <PipelineBoard
              leads={filteredLeads}
              onLeadClick={(lead) => {
                setSelectedLead(lead)
                setDrawerOpen(true)
              }}
            />

            <AIPriorities
              priorities={priorities}
              onLeadClick={(lead) => {
                setSelectedLead(lead)
                setDrawerOpen(true)
              }}
            />
          </>
        )}

        {activeView === "tasks" && (
          <TaskCenter
            tasks={tasks}
            automationRules={automationRules}
            onLeadClick={(id) => {
              // make id matching robust (string vs number)
              const lead = leads.find((l) => String(l.id) === String(id))
              if (lead) {
                setSelectedLead(lead)
                setDrawerOpen(true)
              }
            }}
          />
        )}
      </div>

      <LeadDrawer
        lead={selectedLead}
        open={drawerOpen}
        onClose={() => setDrawerOpen(false)}
      />
    </div>
  )
}