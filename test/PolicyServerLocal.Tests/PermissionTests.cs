﻿using System;
using FluentAssertions;
using Xunit;

namespace PolicyServerLocal.Tests
{
    public class PermissionTests
    {
        Permission _subject;

        public PermissionTests()
        {
            _subject = new Permission();
        }

        [Fact]
        public void Evaluate_should_require_roles()
        {
            Action a = () => _subject.Evaluate(null);
            a.ShouldThrow<ArgumentNullException>();
        }

        [Fact]
        public void Evaluate_should_fail_for_invalid_roles()
        {
            var result = _subject.Evaluate(new[] { "foo" });
            result.Should().BeFalse();
        }

        [Fact]
        public void Evaluate_should_succeed_for_valid_roles()
        {
            _subject.Roles.Add("foo");
            var result = _subject.Evaluate(new[] { "foo" });
            result.Should().BeTrue();
        }
    }
}