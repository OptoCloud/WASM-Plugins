#include <emscripten.h>
#include <string.h>

#define MOD_FUNC extern "C" EMSCRIPTEN_KEEPALIVE

MOD_FUNC void _consoleLog(const char *message, int length);
void ConsoleLog(const char* message) {
  _consoleLog(message, strlen(message));
}

MOD_FUNC
void OnLoad() {
  ConsoleLog("WASM Module Loaded");
}

MOD_FUNC
void OnUnload() {
  ConsoleLog("WASM Module Unloaded");
}

MOD_FUNC
void OnConnected() {
  ConsoleLog("Connected to server");
}

MOD_FUNC
void OnDisconnected() {
  ConsoleLog("Disconnected from server");
}