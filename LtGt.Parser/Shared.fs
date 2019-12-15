module internal LtGt.Parsers.Shared

open FParsec

let (<&>) f g = (fun x -> f x && g x)

let isSpace = FParsec.Text.IsWhitespace

let isNotSpace = isSpace >> not

let trim (s : System.String) = s.Trim()

let htmlDecode = System.Net.WebUtility.HtmlDecode

let letterOrDigit : Parser<char, unit> =
    choice [
        letter
        digit
    ]

let manyCharsBetween popen pchar pclose =
    popen >>. manyCharsTill pchar pclose