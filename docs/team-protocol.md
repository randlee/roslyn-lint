# Team Messaging Protocol (Dogfooding)

This protocol is mandatory for all ATM team communications.

## Required Flow

1. Immediately reply to every ATM message received.
- If the task is starting now, say so explicitly.
- If the task is queued behind active work, say that explicitly.
2. Do not use `atm ack` for a queued task.
3. Run `atm ack` only when the task becomes the active task and work is starting.
4. Execute the requested task.
5. Send a completion message with a concise summary of what was done.
- Example: `task complete: <summary>`
6. Receiver immediately acknowledges completion.
7. No silent processing. Every message must receive a response.

## Good Patterns

- Request received:
  - `received DEV-S2 queued after DEV-S1.`
- Task start:
  - `atm ack DEV-S2`
  - `starting DEV-S2 on fix/s2-worktree now.`
- Start decision:
  - if another task is already active, send a queued receipt and continue the active task
  - if no task is active, run `atm ack` and send the start message before executing
- Completion sent:
  - `task complete: rebased on integrate/phase-E, resolved socket.rs conflict, tests passed, pushed 2f190f3.`
- Completion acknowledged:
  - `received. QA pass starting now.`

## Bad Patterns

- Reading a task message and doing work without sending an ack.
- Running `atm ack` for a task that is only queued and has not started.
- Sending only a final message with no initial acknowledgement.
- Sending a status update without clear completion or next action.
- Letting a message sit without response while processing internally.

## Notes

- If blocked, send an immediate reply plus blocker status.
- If work will take time, send periodic progress updates.
- Prefer concise, explicit messages with branch/commit/test context when relevant.
- Queue order matters. Unless a task explicitly interrupts current work, reply that it is queued and continue the active task.
- During phased work, queue the next known task as soon as the current task is started.
