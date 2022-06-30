This code was ported from https://github.com/andrewfry/SharpML-Recurrent.git

The back propagation code was changed from IRunnable/delegate to IBackpropOperation. 
Graph was split into prediction (forward) and training (backward) implementations. 