namespace TestAnalyzers
{
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Threading.Tasks;

    public record Header(string name, string value);

    public record Request(HttpMethod method, string uri, Version version, List<Header> headers)
        : RequestStartLine(method, uri, version);

    public enum ParsingStatus
    {
        /// <summary>
        /// Successfull parsing
        /// </summary>
        Success,

        /// <summary>
        /// Failed parsing
        /// </summary>
        WrongMessageFormat,

        /// <summary>
        /// Got unsupported version
        /// </summary>
        VersionNotSupported,

        /// <summary>
        /// Illegal, unsupported method
        /// </summary>
        WrongMethod,
    }

    public record RequestStartLine(HttpMethod method, string uri, Version version);

    public record ParsingResult(ParsingStatus status, RequestStartLine line);

    public record SuccessfulParsingResult(RequestStartLine line) : ParsingResult(ParsingStatus.Success, line);

    public record FailedParsingResult(ParsingStatus status) : ParsingResult(status, line: null);

    public interface IPacketParser
    {
        Task<Request> ParseRequestHeadersAsync(ParsingResult parsedFirstLine);

        Task<ParsingResult> ParseRequestStartLineAsync();
    }
}
