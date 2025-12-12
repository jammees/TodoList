<div align="center">
  <img src="./Assets/todolist_wide.png" alt="Postprocessable Razor Panel Logo">
</div>

<br>

A simple todo widget to keep track of things in the editor. Saves to each project. The library allows to import and export the entries in case the cookies that are used to
save the data expire or to have different branches of todos.

<br>

# Installation

The library can be found in the s&box library manager! It is recommended to install the library from there!

<br>

# How to Use

> [!TIP]
> All of the widgets can be easily closed by pressing `ESC`, while
> they are in focus. This will not save the changes.

> [!TIP]
> There's a help window located in the more menu to quickly reference!

Once opening the widget (can be found inside of __View -> Todos__), new entries can be added
by using the __Add new entry__ button. There, the todo message can be written and a group can be chosen. Each group
is categorised alphabetically and allows to collapse all entries under them.

Once done, clicking on an entry will toggle its completion state. The group the entry is under
will display the progress: (__currently done__/__all entries__).

<br>

## Search bar

### Flags

The search bar has flags now! All flags follow the same pattern of **FLAG_NAME:ARGUMENT**. Some do not require an argument but still need the **":"** to be recognized as a flag. Multiple flags can be added at the same time, while they're separated by spaces. Anything else that's not a flag will be used to look directly at the message and see if it contains them.

<br>

#### Built-in flags

| Name          	| Argument		| For:			| Description:									|
| ------------	|----------------- |----------------- |------------------------------------------------------	|
| in                 	| Group Name	| Manual/Code	| Filters entries based on if they're in a group		|
| not              	| Group Name	| Manual/Code	| Filters entries based on if they're not in that group	|
| done           	| None			| Manual			| Is the entry marked as done						|
| pending      	| None			| Manual			| Is the entry marked as not done					|

<br>

#### Code entries

To look for entries that have a specific code word, all that needs to be done is write down the name of the code word that was defined in the settings and follow it with a **":"** if not there already.

For example: `todo:`

<br>

# Editing

### Entry

To edit an entry <kbd>Shift</kbd> + <kbd>Click</kbd> on it. A similar window will pop-up where the message can
be rewritten and a new group can be picked. The window also allows deleting the same entry if necessary.

<br>

### Group

To edit a group of entries <kbd>Shift</kbd> + <kbd>Click</kbd> on the group name itself. This window
only allows changing all the entries' group to something else or to delete all of them.

<br>

# Code

The library now imports code markups ( here, referred to as _code word_s )!
By default, they are hidden but can be toggled either with the eye icon, next
to the __Add new entry__ button.

<img width="259" height="102" alt="image" src="https://github.com/user-attachments/assets/6272373b-434d-416d-93fa-a6604162a8f8" />

Here are a list of default _code words_:

- todo
- bug
- fixme
- xxx
- note
- optimize
- hack

Can be changed in the [Settings](#settings).

### Example:

```cs
// note: this is a class
public sealed class MyClass
{
  public float MyValue { get; set; } = 1f; // todo: change default value

  // fIxMe: fix this really
  // incosistent naming, also this is
  // mutliline!
  public void My_Method_1()
  {
    var myValue; // works even in here
  }

  // TODO: this is a todo
  // that spans mutliple lines
  //
  // I'm just a simple comment
  //
  // TODO: this a different todo
  public void MyMethod1()
  {

  }
}
```

<br>

# Settings

There are a couple of settings that can be changed. Most notably,
adding or removing _code words_. Can be found in the more menu, next to
the eye icon.

<br>

### Refresh on Hotload

Redraws everything in the list if __Show Code Entries__ are toggled on
hotload.

<br>

### Widgets Stay on Top

> [!WARNING]
> This setting has been removed since v131258 in favour of
> the widgets always staying on top of the editor instead!

Makes all widgets to have the stay on top flag. Makes widgets
behave more nicely if something needs to be copied to them
from a different part of the editor, as they won't go behind
the editor.

In return, dialogs will appear behind them!

<br>

### Uncollapse on Search

Makes all the groups uncollapse when searching and will block
all attemps at trying to open or close them.

<br>

### Default Group Name

> [!NOTE]
> If the group is invalid ( empty ) it will still result
> with the _"Default"_ group

Allows to set a default group name to be used when creating
a new entry instead of _"Default"_.

<br>

### Code Words

Here can all of the _code words_ the library will check against can be edited, removed
or added. If required, pressing and confirming __Reset All__ will set the default _code words_.

> [!IMPORTANT]
> When creating a new one, make sure it ends with a ":". They are not
> required, but makes it consistent with the rest and avoids false
> positives!

<br>

# Keybinds

There are a couple of keybinds available right now:

| Keybind     | Description:                         |
|-------------|------------------------------------  |
| `CTRL`+`W`  | Opens the create a new entry dialog  |
| `CTRL`+`E`  | Toggles show manual entries          |
| `CTRL`+`R`  | Toggles show code entries            |

<br>

# Import/Export

Just in case, the library allows importing and exporting its entries in case the cookies that hold the
data expire, something else happens or to switch to a different set of todos.

These options can be found in the more menu, next to the eye icon. All entries are encoded as a UTF-8 JSON.
