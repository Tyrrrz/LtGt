module internal LtGt.Parsers.Shared

open FParsec

let (<&>) f g = (fun x -> f x && g x)

let isSpace = System.Char.IsWhiteSpace

let isNotSpace = isSpace >> not

let trim (s : System.String) = s.Trim()

let htmlDecode = System.Net.WebUtility.HtmlDecode

let letterOrDigit : Parser<char, unit> =
    choice [
        letter
        digit
    ]

let manyCharsBetween pchar popen pclose =
    popen >>. manyCharsTill pchar pclose