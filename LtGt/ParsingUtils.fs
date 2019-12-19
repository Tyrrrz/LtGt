namespace LtGt

open FParsec

module ParsingUtils =

    let isSpace = FParsec.Text.IsWhitespace

    let isNotSpace = isSpace >> not

    let letterOrDigit : Parser<char, unit> =
        choice [
            letter
            digit
        ]

    let manyCharsBetween popen pchar pclose =
        popen >>. manyCharsTill pchar pclose

    let many1CharsBetween popen pchar pclose =
        popen >>. many1CharsTill pchar pclose

    let anyStringOfCI strings =
        strings
        |> Seq.map pstringCI
        |> choice