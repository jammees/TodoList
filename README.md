<div align="center">
  <img src="./Assets/todolist_wide.png" alt="Postprocessable Razor Panel Logo">
</div>

<br>

A simple todo widget to keep track of things in the editor. Saves to each project. The library allows to import and export the entries in case the cookies that are used to
save the data expire or to have different branches of todos.

# Installation

The library can be found in the s&box library manager! It is recommended to install the library from there!

<br>

# How to Use

Once opening the widget (can be found inside of __View -> Todos__), new entries can be added
by using the __Add new entry__ button. There, the todo message can be written and a group can be chosen. Each group
is categorised alphabetically and allows to collapse all entries under them.

Once done, clicking on an entry will toggle its completion state. The group the entry is under
will display the progress: (__currently done__/__all entries__).

If there are a lot of entries present, above the __Add new entry__ button is a _searchbar_. It is
very simple but it should do its job.

<br>

# Editing

### Entry

To edit an entry <kbd>Shift</kbd> + <kbd>Click</kbd> on it. A similar window will pop-up where the message can
be rewritten and a new group can be picked. The window also allows deleting the same entry if necessary.

### Group

To edit a group of entries <kbd>Shift</kbd> + <kbd>Click</kbd> on the group name itself. This window
only allows changing all the entries' group to something else or to delete all of them.

<br>

# Import/Export

Just in case, the library allows importing and exporting its entries in case the cookies that hold the
data expire, something else happens or to switch to a different set of todos.

These options can be found in the top bar under __Todo List__. All entries are encoded as a UTF-8 JSON.
