// src/components/ai-priorities.tsx
"use client"

import type { Lead } from "@/lib/mock-data"
import { Badge } from "@/components/ui/badge"

export function AIPriorities({
  priorities,
  onLeadClick,
}: {
  priorities: Lead[]
  onLeadClick: (lead: Lead) => void
}) {
  return (
    <aside className="w-[360px] shrink-0 border-l p-4 overflow-y-auto">
      <div className="mb-3 text-sm font-medium">AI Priorities</div>

      <div className="space-y-3">
        {priorities.map((lead) => (
          <button
            key={String(lead.id)}
            onClick={() => onLeadClick(lead)}
            className="w-full rounded-lg border p-3 text-left hover:bg-muted/40"
          >
            <div className="flex items-start justify-between gap-2">
              <div>
                <div className="text-sm font-medium">{lead.name}</div>
                <div className="text-xs text-muted-foreground">{lead.company}</div>
              </div>

              <div className="text-xs text-muted-foreground">
                Score: {lead.score ?? 0}
              </div>
            </div>

            {/* ✅ SAFE: aiReasons might be missing */}
            <div className="mt-2 flex flex-wrap gap-1">
              {(Array.isArray((lead as any).aiReasons) ? (lead as any).aiReasons : []).map(
                (reason: string) => (
                  <Badge key={reason} variant="outline">
                    {reason}
                  </Badge>
                )
              )}

              {/* optional: nice fallback chip */}
              {(!Array.isArray((lead as any).aiReasons) || (lead as any).aiReasons.length === 0) && (
                <Badge variant="outline">No AI reasons yet</Badge>
              )}
            </div>
          </button>
        ))}
      </div>
    </aside>
  )
}