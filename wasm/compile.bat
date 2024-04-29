@ECHO OFF

emcc.bat -s ERROR_ON_UNDEFINED_SYMBOLS=0 -g0 -O3 -o output.wasm --no-entry .\main.cpp