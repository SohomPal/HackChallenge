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
        const data = await api.getLeads()

        /**
         * IMPORTANT:
         * Backend shape may differ from UI Lead type.
         * Map here if needed.
         *
         * Adjust fields ONLY if your API differs.
         */
        // @ts-ignore
        const mapped: Lead[] = data.map((l: any) => ({
          id: l.id,
          name: l.name,
          company: l.company,
          stage: l.stage ?? "new",
          score: l.score ?? 0,
          lastContact: l.lastContact ?? "",
          owner: l.owner ?? "",
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
   */
  useEffect(() => {
    if (!leads.length) return

    async function loadPriorities() {
      try {
        // simple strategy → top 5 leads
        const subset = leads.slice(0, 5)

        await Promise.all(
          subset.map((l) =>
            api.getNextActions(Number(l.id)).catch(() => null) // don't crash page
          )
        )

        setPriorities(subset)
      } catch (e) {
        console.error(e)
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
        lead.name.toLowerCase().includes(q) ||
        lead.company.toLowerCase().includes(q)
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
              const lead = leads.find((l) => l.id === id)
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