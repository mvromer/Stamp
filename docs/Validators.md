# Validators

A Stamp parameter can define one or more validators. Each validator ensures the value defined for
that parameter is valid with respect to some well-defined criteria that are checked at runtime. All
validators must pass for a value to be considered valid.

## Supported validators

The following are the list of validators supported by Stamp:

* Choice - Ensures a parameter value is equal to one of the values in a predefined set.
* Script - Defines a custom validator. The script must evaluate to true if the parameter value is
  valid. Otherwise, the script must evaluate to false.

## Choice validator
A choice validator defines a set of values. A parameter value is valid with respect to a choice
validator if the value is equal to one of the values in the choice validator's value set. Each value
defined by the choice validator must be convertible to the data type of the associated parameter.

## Script validator
TBD.
