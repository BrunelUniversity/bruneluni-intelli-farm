﻿SELECT MAXBOUNCES, ROUND(RENDERTIMESECONDS, 0), PolyCount, UtcCurrent AS TIME
FROM FeasabilityDtos
  WHERE Coverage = 100
  AND SAMPLES = 100
ORDER BY MaxBounces