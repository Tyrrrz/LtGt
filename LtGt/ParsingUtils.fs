namespace LtGt

open FParsec

module internal ParsingUtils =

    let isSpace = FParsec.Text.IsWhitespace

    let isNotSpace = isSpace >> not

    let letterOrDigit : Parser<char, unit> =
        choice [
            letter
            digit
        ]

    let inline manyCharsBetween popen pchar pclose =
        popen >>. manyCharsTill pchar pclose

    let inline many1CharsBetween popen pchar pclose =
        popen >>. many1CharsTill pchar pclose

    let inline anyStringOfCI strings =
        strings
        |> Seq.map pstringCI
        |> choice

    /// Runs parser on source and produces a result union.
    let inline runWithResult parser source =
        match run parser source with
        | Success (res, _, _) -> Result.Ok res
        | Failure (err, _, _) -> Result.Error err