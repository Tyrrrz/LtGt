namespace LtGt

open FParsec

// Exception thrown when parsing fails.
// Used as an alternative to Result union when called from C#.
exception ParseException of string

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

    /// Runs parser on source and either produces a successful result or throws an exception.
    let inline runOrRaise parser source =
        match run parser source with
        | Success (res, _, _) -> res
        | Failure (err, _, _) -> raise (ParseException err)