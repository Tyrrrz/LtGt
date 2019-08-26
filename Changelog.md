### v1.0.2 (26-Aug-2019)

- Fixed an issue where left-recursive contextual combinators didn't work in CSS selectors. For example, now you can do `#elem > h3 > span.yellow p a[href]`.
- Fixed an issue where escaped special characters were still considered invalid in CSS selectors. For example, now you can do `div.foo\(bar\)` to match elements with class name `foo(bar)`.

### v1.0.1 (10-Jul-2019)

- Fixed an issue where some valid unquoted attribute values were not parsed correctly.