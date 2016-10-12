﻿using System.Linq;
using System.Text;
using IntegratedAuthoringTool.DTOs;

namespace IntegratedAuthoringTool
{
	public static class DialogUtilities
	{
		private static string JoinStringArray(string[] strs)
		{
			switch (strs.Length)
			{
				case 0:
					return "-";
				case 1:
					return strs[0];
			}

			return strs.Aggregate((s, s1) => s + "," + s1);
		}

		public static string GenerateFileKey(DialogueStateActionDTO dto)
		{
			return $"{dto.CurrentState}#{dto.NextState}#{JoinStringArray(dto.Meaning)}({JoinStringArray(dto.Style)})".ToUpperInvariant();
		}

		public static uint UtteranceHash(string utterance)
		{
			uint hash = 0;
			var bytes = Encoding.UTF8.GetBytes(utterance);
			for (var i = 0; i < bytes.Length; i++)
			{
				var move = i%32;
				var h = (uint) (bytes[i] << move);
				hash ^= h;
			}
			return hash;
		}
	}
}