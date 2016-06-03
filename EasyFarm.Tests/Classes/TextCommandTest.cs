using System;
using Xunit;

namespace EasyFarm.Tests.Classes
{
    public class TextCommandTest
    {
        private static class TextCommands
        {
            public static readonly TextCommand Potion = new TextCommand(@"/item ""Potion"" <me>");
            public static readonly TextCommand Fire = new TextCommand(@"/ma ""Fire"" <t>");
            //public static readonly TextCommand WaitCommand = new TextCommand(1000);
        }

        [Theory, AutoMoqData]
        public void SelfTextCommandReturnsCorrectResult()
        {
            var sut = TextCommands.Potion;
            Assert.Equal(TargetType.Self, sut.TargetType);
        }

        [Theory, AutoMoqData]
        public void TargetTextCommandReturnsCorrectResult()
        {
            var sut = TextCommands.Fire;
            Assert.Equal(TargetType.Target, sut.TargetType);
        }

        [Fact]
        public void TwoPartTargetTextCommandReturnsCorrectResult()
        {
            var sut = new TextCommand("/target <t>");
            Assert.Equal(TargetType.Target, sut.TargetType);
        }

        //[Fact]
        //public void CompositeCommandReturnsCorrectResult()
        //{
        //    var sut = new CompositeCommand(
        //        TextCommands.TargetCommand(anonymousTargetId),
        //        TextCommands.WaitCommand(1000),
        //        TextCommands.Fire);
        //}
    }

    public enum TargetType
    {
        Unknown,
        Self,
        Target
    }

    public class TextCommand
    {
        public TextCommand(string command)
        {
            var splits = command.Split(new[] {" "}, StringSplitOptions.RemoveEmptyEntries);

            string rawTargetType = "";

            switch (splits.Length)
            {
                case 3:
                    rawTargetType = splits[2];
                    break;
                case 2:
                    rawTargetType = splits[1];
                    break;
            }

            TargetType = ParseTargetType(rawTargetType);
        }

        private TargetType ParseTargetType(string rawTargetType)
        {
            if (rawTargetType.Contains("me")) return TargetType.Self;
            if (rawTargetType.Contains("t")) return TargetType.Target;
            return TargetType.Unknown;
        }

        public TargetType TargetType { get; }
    }
}