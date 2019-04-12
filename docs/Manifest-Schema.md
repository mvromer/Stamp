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
    constraints: "Must be one of the following: int, float, string, bool."

  - name: validator
    type: mapping
    required: false
    constraints: "Value must be one of the following mappings: choice."
    children:
    - name: choice
      type: list of values
      required: true
      constraints: Each value must be convertible to parameter's type.

  - name: required
    type: bool
    required: false
    default: true
```
