# Stamp
Command line template generator that is:

* Cross-platform
* Language agnostic
* Extensible

## How it works
When Stamp is used to create a new instance of a template, it reads and processes the template's
manifest. It first defines values for all parameters listed in the manifest. The user will be
prompted to provide values for any required parameters whose values cannot otherwise be determined.

After defining all parameters, Stamp will create the output directory if it does not already exist.
By default, STamp writes all output to the current directory.

After the output directory is created, Stamp will run any pre-hook scripts defined by the manifest.

After the pre-hook scripts successfully run, Stamp will begin processing all template files listed
in the manifest. Stamp will process a template file in one of two depending on how it is defined by
the manifest:

* Verbatim - These files will be copied to the output directory with no transformations applied to
  their contents.
* Computed - These files can contain substitution expressions. For each substitution expression
  Stamp finds in a file, it will evaluate the expression and use its value to replace the contents
  of the expression in the template file. The transformed file is then copied to the output
  directory.

When Stamp copies a file to the output directory, by default it will place it in the same directory
relative to the template's root directory. For example, if a template has a file located at
`<template root>/foo/example.xml`, then it will copy this file to the location
`<output directory>/foo/example.xml`. This behavior can be configured on a per file basis in the
template's manifest.

After all template files have been processed, Stamp will run any post-hook scripts defined by the
manifest. Once all post-hook scripts are complete, Stamp terminates.

## Commands
* `new <template name>` - Generate a new instance of the named template.
  * `--output <output directory>` - Specify the output directory for the new instance.
* `repo [list]` - List all configured repositories.
  * `repo add <repo URL> [<repo name>]` - Configures a new repository. If no name is given, then it
    is pulled from the repo's URL. The name must be a single word and unique amongst all previously
    registered repositories.
    * `--trust` - Add the repository as a trusted repository.
  * `repo set <repo name> <setting name> <setting value>` - Updates a setting for the named
    repository. The following are valid names of settings that can be configured:
    * `url` - The Git URL of the named repository.
    * `trust` - Boolean specifying whether the repo is trusted or not. Valid values for this setting
      are `true` or `false`.
* `update` - Updates templates for all configured repositories.
  * `update <repo>` - Updates templates in only the named repo.

## Template versioning
Each template is semantically versioned. At this time, no support for prerelease versions is
provided.

## Template repository
Stamp pulls all templates it knows about from one or more Git repositories. Each Stamp repo is
configured with the following settings:

* Repository URL - Git URL Stamp will use when cloning the repo and pulling updates from it.
* Repository Name - A single word identifier used to refer to the repo in various Stamp commands.
  The name must be unique amongst all repositories configured by Stamp.

A Stamp repo is expected to have the following layout:

```
<repo root>
   |- templates/
      |- BarTemplateV1/
      |- FooTemplateV2/
      |- FooTemplateV3/
            .
            .
            .
```

At the root is a directory named `templates` that stores each of the templates sourced by that
repository. Each template is stored in its own directory named `<template name>V<major version>`.

## Template structure
Each template folder has the following structure:

```
<template root>
   |- .hooks/
      |- pre/
      |- post/
   |- manifest.yml
   |- <other template files defined in the manifest>
```

## Manifest file
A manifest file has the following structure:

```yaml
name: FooTemplate
version: 1.2.3

parameters:
- name: projectName
  type: string
  required: true

files:
- path: 'run.sh'

- path: 'config/foo.xml'
  computed: true

- path: 'data/bar.csv'
  computed: true
  outputDir: '{{projectName}}Data'
  outputName: 'customData.csv'

pre:
- type: inline
  script: 'return foo()'

post:
- type: file
  script: 'run-post.js'
```

## Configuration
Stamp configuration is stored under Environment.SpecialFolder.AppData. Configuration includes:

* Repository config file that includes:
   * A list of template repositories previously configured
   * For each template repository, the following settings are stored:
      * Repo name
      * Repo URL
      * Repo trust status (trusted or untrusted)
      * Repo access token (if authentication is needed to clone/fetch)
      * A list of trusted templates used when the repo is untrusted. Each entry specifies:
         * Template name
         * (TBD) Hash of template files
      * A list of untrusted templates used when the repo is trusted. Each entry specifies:
         * Template name
* A `repos` directory that contains a clone of each template repository. Each repository is stored
  in a directory named after the repo's name.

NOTE: The rationale for possibly including a hash with the name of a trusted template is to verify
the template's contents did not change.

## Trust model
Templates can execute arbitrary code through substitution expressions and hook scripts. For this
reason, a template must be marked as trusted before it can be instantiated by Stamp. By default, all
templates are implicitly untrusted.

When Stamp attempts to instantiate an untrusted template, it will prompt the user with a list of
choices:

* View the template's contents - This will display all executable code segments in the template.
* Trust the template for the duration of the current run only and continue execution.
* Mark the template as trusted and continue execution.

## Scripting
Support for custom scripting is exposed through both substitution expressions and hook scripts. A
template author will define both of these aspects using a scripting language supported by Stamp.

Stamp will support C# scripting for both usages. Internally, this will be built on top of the
Rosyln scripting API for the initial release. 
