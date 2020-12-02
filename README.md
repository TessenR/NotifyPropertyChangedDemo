# NotifyPropertyChangedDemo
Demo generator implementing INotifyPropertyChanged based on naming conventions

A very simple generator demonstrating the basic functioning of a source generator.
The example features a generator implementing INotifyProperytyChanged interface based on naming convention adding properties with PropertyChanged event invocation for any field with *BackingField suffix in all types implementing INotifyPropertyChanged interface
- How to add a source generator
- How to traverse syntax trees, check implemented interfaces and find type members
- How to add a generated source to the target project
- How to test a source generator
- How to debug a source generator

Note that being a demo this generator completely ignores some potential problems in generated source e.g. non-class types implementing INotifyPropertyChanged, nested types or types in global namespaces focusing instead on being as simple an example as possible
