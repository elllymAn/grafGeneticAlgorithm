// dll.cpp : Defines the exported functions for the DLL.
//

#include "pch.h"
#include "framework.h"
#include "dll.h"


// This is an example of an exported variable
DLL_API void geneticModify(const char* input, char* output, int lengthOut)
{
	std::string in = std::string(input);
	Graph* g = new Graph(in);
	std::string* out = new std::string(g->geneticAlgorithm());
	DB* db = new DB(*out);
	std::ofstream file("log.txt");
	db->returnLog(file);
	file.close();
	delete db;
	delete g;
	strncpy_s(output, lengthOut, (out)->c_str(), lengthOut);
	delete out;
}
