# Testing commands

## 1000 points 128x128x128
```
PS C:\Git\Masters\nrg\InterpolationShepard\bin\Debug\net6.0> .\InterpolationShepard.exe --input point-cloud-1k.raw --output point-clous-1k.bin --method basic --p 2.0 --min-x 0.0 --min-y 0.0 --min-z 0.0 --max-x
 1.0 --max-y 1.0 --max-z 1.0 --res-x 128 --res-y 128 --res-z 128
LOG: Output file: point-clous-1k.bin
LOG: Arguments set
LOG: Loaded data with path: point-cloud-1k.raw
LOG: Break for point Point(0,29635528, 0,007666542, 0,12538223):0 and volume point Point(0,296875, 0,0078125, 0,125):0
LOG: Break for point Point(0,179663, 0,53144896, 0,1248835):0,8392157 and volume point Point(0,1796875, 0,53125, 0,125):0
LOG: Break for point Point(0,17197457, 0,8206996, 0,6486532):0,4117647 and volume point Point(0,171875, 0,8203125, 0,6484375):0
LOG: Break for point Point(0,91389984, 0,008201737, 0,7036943):0 and volume point Point(0,9140625, 0,0078125, 0,703125):0
LOG: Break for point Point(0,74233145, 0,3205683, 0,76616573):0,6784314 and volume point Point(0,7421875, 0,3203125, 0,765625):0
LOG: Break for point Point(0,9291033, 0,6642172, 0,7741135):0,33333334 and volume point Point(0,9296875, 0,6640625, 0,7734375):0
LOG: Break for point Point(0,42158052, 0,3361146, 0,8290521):0,7529412 and volume point Point(0,421875, 0,3359375, 0,828125):0
LOG: Interpolated and wrote data to output file with elapsed time of 00:00:58.7117132
LOG: Writing to visualization file file...DONE, exiting
PS C:\Git\Masters\nrg\InterpolationShepard\bin\Debug\net6.0> .\InterpolationShepard.exe --input point-cloud-1k.raw --output point-clous-1k.bin --method modified --p 2.0 --min-x 0.0 --min-y 0.0 --min-z 0.0 --max-x 1.0 --max-y 1.0 --max-z 1.0 --res-x 128 --res-y 128 --res-z 128
LOG: Output file: point-clous-1k.bin
LOG: Arguments set
LOG: Loaded data with path: point-cloud-1k.raw
LOG: Initialized octree max with depth of 8
LOG: Break for point Point(0,29635528, 0,007666542, 0,12538223):0 and volume point Point(0,296875, 0,0078125, 0,125):0
LOG: Break for point Point(0,179663, 0,53144896, 0,1248835):0,8392157 and volume point Point(0,1796875, 0,53125, 0,125):0
LOG: Break for point Point(0,17197457, 0,8206996, 0,6486532):0,4117647 and volume point Point(0,171875, 0,8203125, 0,6484375):0
LOG: Break for point Point(0,91389984, 0,008201737, 0,7036943):0 and volume point Point(0,9140625, 0,0078125, 0,703125):0
LOG: Break for point Point(0,74233145, 0,3205683, 0,76616573):0,6784314 and volume point Point(0,7421875, 0,3203125, 0,765625):0
LOG: Break for point Point(0,9291033, 0,6642172, 0,7741135):0,33333334 and volume point Point(0,9296875, 0,6640625, 0,7734375):0
LOG: Break for point Point(0,42158052, 0,3361146, 0,8290521):0,7529412 and volume point Point(0,421875, 0,3359375, 0,828125):0
LOG: Interpolated and wrote data to output file with elapsed time of 00:00:21.5972624
LOG: Writing to visualization file file...DONE, exiting
```

Speedup: 2.761904761904762

## 1000 points 128x128x128
Changed p and R
```
PS C:\Git\Masters\nrg\InterpolationShepard\builds\First> .\InterpolationShepard.exe --input point-cloud-1k.raw --output output-1k.raw --method modified --R 0.5 --p 10.0 --min-x 0.0 --min-y 0.0 --min-z 0.0 --max-x 1.0 --max-y 1.0 --max-z 1.0 --res-x 128 --res-y 128 --res-z 128
LOG: Output file: output-1k.raw
LOG: Arguments set
LOG: Loaded data with path: point-cloud-1k.raw
LOG: Initialized octree max with depth of 8
LOG: Break for point Point(0,29635528, 0,007666542, 0,12538223):0 and volume point Point(0,296875, 0,0078125, 0,125):0
LOG: Break for point Point(0,179663, 0,53144896, 0,1248835):0,8392157 and volume point Point(0,1796875, 0,53125, 0,125):0
LOG: Break for point Point(0,17197457, 0,8206996, 0,6486532):0,4117647 and volume point Point(0,171875, 0,8203125, 0,6484375):0
LOG: Break for point Point(0,91389984, 0,008201737, 0,7036943):0 and volume point Point(0,9140625, 0,0078125, 0,703125):0
LOG: Break for point Point(0,74233145, 0,3205683, 0,76616573):0,6784314 and volume point Point(0,7421875, 0,3203125, 0,765625):0
LOG: Break for point Point(0,9291033, 0,6642172, 0,7741135):0,33333334 and volume point Point(0,9296875, 0,6640625, 0,7734375):0
LOG: Break for point Point(0,42158052, 0,3361146, 0,8290521):0,7529412 and volume point Point(0,421875, 0,3359375, 0,828125):0
LOG: Interpolated and wrote data to output file with elapsed time of 00:00:07.1783233
LOG: Writing to visualization file file...DONE, exiting
PS C:\Git\Masters\nrg\InterpolationShepard\builds\First> .\InterpolationShepard.exe --input point-cloud-1k.raw --output output-1k.raw --method basic --R 0.5 --p 10.0 --min-x 0.0 --min-y 0.0 --min-z 0.0 --max-x 1.0 --max-y 1.0 --max-z 1.0 --res-x 128 --res-y 128 --res-z 128
LOG: Output file: output-1k.raw
LOG: Arguments set
LOG: Loaded data with path: point-cloud-1k.raw
LOG: Break for point Point(0,29635528, 0,007666542, 0,12538223):0 and volume point Point(0,296875, 0,0078125, 0,125):0
LOG: Break for point Point(0,179663, 0,53144896, 0,1248835):0,8392157 and volume point Point(0,1796875, 0,53125, 0,125):0
LOG: Break for point Point(0,17197457, 0,8206996, 0,6486532):0,4117647 and volume point Point(0,171875, 0,8203125, 0,6484375):0
LOG: Break for point Point(0,91389984, 0,008201737, 0,7036943):0 and volume point Point(0,9140625, 0,0078125, 0,703125):0
LOG: Break for point Point(0,74233145, 0,3205683, 0,76616573):0,6784314 and volume point Point(0,7421875, 0,3203125, 0,765625):0
LOG: Break for point Point(0,9291033, 0,6642172, 0,7741135):0,33333334 and volume point Point(0,9296875, 0,6640625, 0,7734375):0
LOG: Break for point Point(0,42158052, 0,3361146, 0,8290521):0,7529412 and volume point Point(0,421875, 0,3359375, 0,828125):0
LOG: Interpolated and wrote data to output file with elapsed time of 00:00:55.4329599
LOG: Writing to visualization file file...DONE, exiting
PS C:\Git\Masters\nrg\InterpolationShepard\builds\First>
```

Speedup: 7.722271285830773