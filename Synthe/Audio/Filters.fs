namespace SyntheAudio
open XPlot.Plotly
open System
open System.IO
open Waves

module Effect =
    let sampleRate = 44100.
    let limit = 44100
    let time = 0.5

    let ByFixedAmount (modifier:float) (wave:float array) =
        for i in 0..(Waves.limit-1) do
            wave.[i] <- wave.[i] * modifier
        wave

    // let newwave = byFixedAmount 3. sinWave
    // newwave |> Chart.Line |> Chart.Show

    let Overdrive (flatten:float) (wave: float array) limit =

        for i in 0..limit-1 do
            if wave.[i] > flatten then
                wave.[i] <- flatten
            elif wave.[i] < 0.0 - flatten then
                wave.[i] <- 0.0 - flatten

        wave

    // let sin = overdrive 0.7 sinWave (limit - 1)

    // sin |> Chart.Line |> Chart.Show


    let Enveloppe time amplitude At De Su Re =

        let Attack = Array.init (int (time*sampleRate*At)) (fun i -> amplitude/(time*sampleRate*At)* float i)
        
        let Decay = Array.init (int (time*sampleRate*De)) (fun i -> amplitude - (amplitude-Su*amplitude)/(time*sampleRate*De) *float i)

        let Sustain = Array.init (int (time*(sampleRate - sampleRate*Re - sampleRate*De - sampleRate*At))) (fun _ -> Su*amplitude)

        let Release = Array.init (int (time*sampleRate*Re)) (fun i -> Su*amplitude - Su*amplitude/(time*sampleRate*Re) * float i)

        let amp = Array.concat [|Attack; Decay; Sustain; Release|]
        amp

    // let Amp = enveloppe 0.5 2 0.1 0.1 0.5 0.1

    // let sinus = Array.init (int(time)*limit) (fun i -> Amp.[i] * sin((freq/ Pi) * float i))

    // Amp |> Chart.Line |> Chart.Show


    let Flange (wave:float array) = 
            [
            let maxTimeDelay = 0.003
            let speed = 1.

            let maxSampleDelay = int (maxTimeDelay * float sampleRate)
            let mutable currentDelay = 0

            let coefficient = 0.5
            let mutable currentSine = 0.

            for i in 0..wave.Length-1 do
                if i < maxSampleDelay+1 then yield wave.[i]
                else
                    currentSine <- abs(sin((Waves.freq/ Waves.Pi) * (float i) * (speed / (float sampleRate))))
                    currentDelay <- int(currentSine * (float maxSampleDelay))
                    yield (coefficient * wave.[i]) + (coefficient * wave.[i-currentDelay])
            ]
    // Flange(sinWave) |> Chart.Line  |> Chart.Show

    let addWaves = Array.map2(fun x y -> (x+y)/2.) Waves.sinWave Waves.squareWave
    // addWaves |> Chart.Line |> Chart.Show

    let Reverb (wave: float []) reduc = 
        let mutable wave2 = wave
        let mutable amp2 = Waves.amp
        while amp2 * reduc > 0.1 do
            amp2 <- amp2 * reduc
            let r = Array.init (Waves.limit) (fun i -> amp2 * sin((2. * Waves.freq * Waves.Pi * float i)/sampleRate))
            let newWave = Array.concat [|wave2; r|]
            wave2 <- newWave
        wave2

    // reverb sinWave 0.6 |> Chart.Line |> Chart.Show

    let Echo (wave: float []) reduc (delay:float) = 
        let mutable wave2 = wave
        let mutable amp2 = Waves.amp
        while amp2 * reduc > 0.1 do
            amp2 <- amp2 * reduc
            let del = Array.init (int(delay * sampleRate))(fun i -> 0.)
            let e = Array.init (Waves.limit) (fun i -> amp2 * sin((2. * Waves.freq * Waves.Pi * float i)/sampleRate))
            let newWave = Array.concat [|wave2; del; e|]
            wave2 <- newWave
        wave2

    // echo sinWave 0.5 0.1 |> Chart.Line |> Chart.Show


    // let AM (wave:float array) maxAmp minAmp =
    //     let mutable multiplicator = amp
    //     while multiplicator in 0..maxAmp do 
    //         if multiplicator < maxAmp then
    //             multiplicator <- multiplicator + 0.1
    //         else

    let AM (wavep: float array) (wavem: float array) = 
        let newWave = Array.init limit (fun i -> wavem.[i] * wavep.[i])
        newWave

    let FM (wavep: float array) (wavem: float array) =
        let newWave = Array.init limit (fun i -> 1. * sin((2. * Pi * 500. * float i) + (1./10.)*(500. - 10.) * wavem.[i]))
        newWave

        
