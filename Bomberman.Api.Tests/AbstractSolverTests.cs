/*-
 * #%L
 * Codenjoy - it's a dojo-like platform from developers to developers.
 * %%
 * Copyright (C) 2020 Codenjoy
 * %%
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as
 * published by the Free Software Foundation, either version 3 of the
 * License, or (at your option) any later version.
 * 
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public
 * License along with this program.  If not, see
 * <http://www.gnu.org/licenses/gpl-3.0.html>.
 * #L%
 */
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Bomberman.Api.Tests
{
    [TestClass]
    public class AbstractSolverTests
    {
        [TestMethod]
        [DataRow("http")]
        [DataRow("https")]
        public void ShouldProvideWebSocketUrlFromServerAddress(string scheme)
        {
            // Arrange.
            var serverUrl = $"{scheme}://unit-test/board/player/0000000001111?code=88888888888";
            var expectedWebSocketUrl = serverUrl.Replace("http", "ws").Replace("board/player/", "ws?user=").Replace("?code=", "&code=");

            var solver = new TestSolver(serverUrl);

            // Act.
            var result = solver.GetWebSocketUrl(serverUrl);

            // Assert.
            Assert.AreEqual(expectedWebSocketUrl, result);
        }
    }
}
