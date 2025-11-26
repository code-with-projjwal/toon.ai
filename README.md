# toon.ai
.net project for AI used for TooN parsing


I can certainly help you write a `README.md` for this C\# class\!

Based on the code, the `ToonParse` class seems to be a utility for converting a **`System.Data.DataTable`** object into a custom string format, likely named "Toon" format, which resembles a structured, escape-encoded CSV or tabular data structure.

Here is the proposed `README.md`:

````markdown
# TooN.AI.ToonParse

A static utility class for serializing a `System.Data.DataTable` object into a custom string format, referred to as the **"Toon" format**.

This format is designed to be a robust, delimited structure that handles special characters, line breaks, and delimiters within the data by using **double quotes** for enclosure and **backslash (`\`)** for escaping.

---

## 🛠️ Usage

The `ToonParse` class provides a single public static method for conversion:

### `FromDataTable(DataTable input)`

Converts the provided `System.Data.DataTable` into the Toon string format.

```csharp
using System.Data;
using Clarity.Business.AI;

// Assuming 'myDataTable' is a populated DataTable object
string toonString = ToonParse.FromDataTable(myDataTable);
````

-----

## 🧭 Toon Format Structure

The generated string follows a specific structure:

1.  **Table Header:** `[TableName][RowCount]{Column1,Column2,...}:`
2.  **Data Rows:** Each row is represented on a new line, containing comma-separated, tokenized cell values.

### 1\. Table Header

  * **`[TableName]`**: The table's name (`input.TableName`), or `"table"` if empty/null. It is always **tokenized** (enclosed in quotes if it contains special characters).
  * **`[RowCount]`**: The number of rows in the table (`input.Rows.Count`).
  * **`{Column1,Column2,...}`**: A comma-separated list of **tokenized** column names. Column names are replaced with `"undefined"` if they are empty/null.
  * **`:`**: A colon separator.
  * **Newline (`\n`)**: Separates the header from the data.

**Example Header:**
`"Customers"[3]{"ID","Name","Address"}:`

### 2\. Data Rows

Each row is a sequence of **tokenized** cell values, separated by a comma (`,`). Rows are separated by a newline (`\n`).

-----

## 📝 Tokenization (Escaping and Enclosing)

The core logic resides in the private static method `tokenize(string token)`. This method ensures that data integrity is maintained even when cell values contain delimiters or escape characters.

The following rules are applied in order:

### 1\. Special Character Escaping

| Character | Replacement | Purpose |
| :---: | :---: | :--- |
| `\` (Backslash) | `\\` | Escapes the escape character itself. |
| `"` (Quote) | `\"` | Escapes the string encloser character. |
| `\n` (Newline) | `\n` | Escapes the newline character. |
| `\r` (Carriage Return) | `\r` | Escapes the carriage return character. |

### 2\. Enclosing (Quoting)

If the token contains *any* of the following characters **after** the above escaping has been performed, the entire token is enclosed in **double quotes (`"`)**:

  * `\` (Backslash)
  * `[` (Left Bracket)
  * `]` (Right Bracket)
  * `{` (Left Brace)
  * `}` (Right Brace)
  * `,` (Comma - the column separator)
  * `:` (Colon - the header separator)

This ensures that tokens containing structural characters are treated as a single unit during parsing.

### Example Tokenization

| Input String | `tokenize()` Output | Notes |
| :---: | :---: | :--- |
| `Hello World` | `Hello World` | No special characters, no quotes needed. |
| `Value,1` | `"Value,1"` | Contains a comma (`,`), so it's quoted. |
| `A line\nbreak` | `"A line\nbreak"` | `\n` is escaped to `\n`, then quoted because `\` is now present. |
| `Path\To\File` | `"Path\\To\\File"` | `\` is escaped to `\\`, then quoted because `\` is now present. |
| `He said "Hi"` | `"He said \"Hi"` | `"` is escaped to `\"`, then quoted because `\` is now present. |
