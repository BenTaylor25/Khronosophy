# To Do List:

## Scheduling

### ETF
- Implementation.

### UTMTK
- Refinements.

### Both
- Hill Climbing optimisation?


## Backend

### REST API
- User
  - Update User Info
    - e.g. Daily intensity capacity
    - Don't include things like names for now
- Taskboard
  - Update task
    - Params: userId, taskId
      - Should validate that taskId belongs to userId,
      but possible optimisation in reading dask directly from DB rather
      than reading all tasks for the user and seraching?
  - Delete task
    - Params: userId, taskId
- Events
  - Update (static) Event
    - Params: userId, eventId
  - Delete (static) Event
    - Params: userId, eventId
  - Delete all scheduled Events for Task
    - Params: userId, taskId
  - Delete all scheduled Events for User
    - Params: userId

### gRPC API
- Decide if gRPC is really worth it.
  - REST would work, it just wouldn't be very semantic.
- Schedule Tasks
  - Dumb Scheduler
  - UTMTK
  - ETF

### Database
- SQLite.


## Frontend

### Tasks Modal
- Update Task info.
  - Pinia stores done, just needs to reference API.
- Delete Task button.
  - Pinia stores done, just needs to reference API.

### Zoom
- Maintain centre point on zoom in and out.

### Display
- Display Events according to UTC time rather than local time?
- Remove '24:00' label from view (like 00:00) so that 24 boxes are shown instead of 25.
- Handle multi-day events by placing a CalendarEvent instance on each day.
- Handle parallel events.

### Tooltips
- Add tooltips to Month view
  - Show date of hover selection.


## ICS Processor(?)

### Logistics
- Do I have time?
- Is it valuable enough?

### ICS File handling
- Import events from .ics file.
- Export events to .ics file.
