namespace LtGt

open FParsec

module internal ParsingUtils =

    let isSpace = FParsec.Text.IsWhitespace

    let isNotSpace = not << isSpace

    let manyCharsBetween popen pclose pchar =
        popen >>. manyCharsTill pchar pclose

    let anyStringBetween popen pclose =
        manyCharsBetween popen pclose anyChar

    let many1CharsBetween popen pclose pchar =
        popen >>. many1CharsTill pchar pclose

    let any1StringBetween popen pclose =
        many1CharsBetween popen pclose anyChar

    let anySinglyQuotedString : Parser<_, unit> = skipChar ''' |> anyStringBetween <| skipChar '''

    let anyDoublyQuotedString : Parser<_, unit> = skipChar '"' |> anyStringBetween <| skipChar '"'

    /// Runs parser on source and produces a result union.
    let runWithResult parser source =
        match run parser source with
        | Success (res, _, _) -> Result.Ok res
        | Failure (err, _, _) -> Result.Error err