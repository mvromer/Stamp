```yaml
- name: name
  type: string
  required: true

- name: version
  type: string
  required: true
  constraints: Must represent a valid SemVer 2.0.0 version number.

- name: parameters
  type: list of mappings
  required: false
  default: Empty list
  itemSchema:
  - name: name
    type: string
    required: true

  - name: type
    type: string
    required: true
    constraints: "Must be one of the following: int, string, bool, choice."
    children:
    - name: choices
      type: list of strings
      required: true
      constraints: Required only if parent type is choice. Each item must be unique.

  - name: required
    type: bool
    required: false
    default: true
```
