Dim message, sapi
message=">mspaint"
Set sapi = CreateObject("sapi.spvoice")
Set sapi.Voice = sapi.GetVoices.Item(1)
sapi.Rate = 0
sapi.volume = 100
sapi.Speak message