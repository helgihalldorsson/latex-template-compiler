# latex-template-compiler

A project made to fill in values in a LaTeX template, to allow the data and template to be saved separately. Made with CVs in mind, but also suitable for commonly used templates (reports, contracts, etc.).

The _example_ directory contains a small [data file](./example/data.json), along with a [multi-file template](./example/template). 

# Requirements
This project requires _pdflatex_ to be installed and available in the command line.

# Data file format
The JSON data file must contain two root objects.
1. The settings for the latex-template-compiler: `"settings": { "ignoreEnabled": <bool>, "supportedLanguages": <list> }`. 
- `ignoreEnabled`: Toggles the 'ignore' keyword in objects. If this setting is enabled, then the compiler will skip items with `"ignore": true` in the data file. This is used for two dates in the [example data file](./example/data.json). Ignore is false by default.
- `supportedLanguages`: This lists the available translations used in the data file. For example, if the supported languages are `["is", "en"]`, then the correct translation will be chosen in objects in the data files of the form `{"is": <value IS>, "en": <value EN>}`, while values without the language 'keys' are invariant to the selected language.

2. The main data must be contained in `"data": {...}"`. This object can be any valid JSON, and no assumptions are made regarding the structure, except the `"ignore"` keyword is reserved if `ignoreEnabled` is enabled in the settings.

# Template format
The placeholder syntax in the template files
1. For single objects, the placeholder must be the path _within the 'data' object_ to the value in the JSON file. It is of the form `<:keyA:keyB:...:keyN:leaf:>`. For example 
```json
{
	"data": {
		"contactInfo": {
			"phone": "(+354) 123 4567"
		}
	}
}
```
Here the placeholder `<:contactInfo:phone:>` would be replaced with "(+354) 123 4567". 

2. List placeholders are made by wrapping a 'placeholder template' with `<:path::>` at the start and `<::path:>` at the end. If each item should be separated with any LaTeX, then it can be placed in `<>{separator}<>` following `<:path::>`. In the example template, the following is used to generate a table with multiple dates. 
```latex
	\begin{tabular}{lll}<:dates::><>\\<>
		<:dates:year:> & <:dates:month:> & <:dates:day:><::dates:>
	\end{tabular}
```
Note that the paths within the list (i.e. `<:dates:year:>`) do not have to state that it is an element in a list.
