# Noot-Scaffold

an scaffold-cli library written in .net6.
It will replace everything files and folders names and it's content following a pattern.
It mimics the usage of g8/scaffold.
## Installation
https://www.nuget.org/packages/noot-scaffold/

`dotnet tool install --global noot-scaffold`
## Usage

Given this structure:

```
.
├── .scaffold/
│   └── entity/
│       ├── src/
│       │   └── {{project;format=package}}/
│       │       └── {{entity;format=camel}}.scala
│       └── scaffold.properties
└── src/
    └── io.github.minze25/
        └── entityOne.scala
```

`nootscaffold entity` or `nootscaffold`

### scaffold.properties file

```properties
description = this is a test scaffold
project=io github minze25
entity=book
```

whitout modifying anything this would create the file `book.scala`

```
.
├── .scaffold/
│   └── entity/
│       ├── src/
│       │   └── {{project;format=package}}/
│       │       └── {{entity;format=camel,lower}}.scala
│       └── scaffold.properties
└── src/
    └── io.github.minze25/
        ├── entityOne.scala
        └── book.scala
```

## Formatting options

Examples with input`hello world.test`

| name    | description          | example result   |
| ------- | -------------------- | ---------------- |
| camel   | Camel Case           | helloWorldTest   |
| package | Replaces with dots   | hello.world.test |
| upper   | Uppercase everything | HELLO WORLD.TEST |
| lower   | Lowercase evertyhing | hello world.test |
| snake   | Snake case           | hello_world_test |
| kebab   | Kebab case           | hello-world-test |
| pascal  | Pascal case          | HelloWorldTest   |

### Composing formatters

You can compose formatters like so: `{{hello;format=package,upper}}`. There are no limits for how many formatters you can compose and they will be applied in order of reading, in this case `hello world.test` would become `HELLO.WORLD.TEST`

### Delimiters

The delimiters for the formatters are `'.', ' ', '_'` 