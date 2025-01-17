﻿namespace SyntheCompress
open System.IO
open SFML.Audio
open System.Threading
open SyntheAudio.Waves
open SyntheAudio.Note

module createSound = 
    let fusedData wave =
        wave |> Array.concat

    let write stream (data:byte[]) =
        let writer = new BinaryWriter(stream)
        // RIFF
        writer.Write("RIFF"B)
        let size = 36 + data.Length in writer.Write(size)
        writer.Write("WAVE"B)
        // fmt
        writer.Write("fmt "B)
        let headerSize = 16 in writer.Write(headerSize)
        let pcmFormat = 1s in writer.Write(pcmFormat)
        let mono = 1s in writer.Write(mono)
        let sampleRate = 44100 in writer.Write(sampleRate) 
        let byteRate = sampleRate in writer.Write(byteRate)
        let blockAlign = 1s in writer.Write(blockAlign)
        let bitsPerSample = 8s in writer.Write(bitsPerSample)
        // data
        writer.Write("data"B)
        writer.Write(data.Length)
        writer.Write(data)

    let sample x = (x + 1.)/2. * 255. |> byte 

    let data = QSilence QuarterSilence 4

    type PlaySound()=
            member x.play stream =
                let buffer = new SoundBuffer(stream:Stream)
                let sound = new Sound(buffer)
                sound.Play()
            
                do while sound.Status = SoundStatus.Playing do 
                    Thread.Sleep(1)
            
    let p = new PlaySound()

            // convert is used to convert data's bytes in stream
    
    let convert = new MemoryStream()
    write convert data

    p.play(convert)