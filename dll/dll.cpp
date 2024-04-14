// dll.cpp : Defines the exported functions for the DLL.
//

#include "pch.h"
#include "framework.h"
#include "dll.h"


// This is an example of an exported variable
DLL_API void geneticModify(const char* input, char* output, char* richText, int lengthOut)
{
	//int* a = new int(5);

	//получение информации о введеном графике формат: имя вершины,имя вершины,...,имя вершины;имя вершины-имя вершины,...,имя вершины-имя вершины(ребра)
	std::string* in = new std::string(input);
	//создаем на основе строки граф
	Graph* g = new Graph(*in);
	//преобразуем граф по генетическому алгоритму и получаем данные об этом графе в виде строки. Вернуться может быть множество графов
	std::vector <Graph> v = (g->geneticAlgorithm());
	// записываем созданные графы в DB
	DB* db = new DB(v);
	std::string out = v[0].codeGraf();
	// записываем результаты в файл
	std::ofstream file("logGraf.txt");
	db->returnLog(file);
	file.close();
	std::ifstream fileR("logGraf.txt");
	std::stringstream buffer;
	buffer << fileR.rdbuf();
	fileR.close();
	std::string str = buffer.str();
	strncpy_s(richText, lengthOut, str.c_str(), lengthOut);
	//очищаем память
	delete db;
	delete g;
	delete in;
	//записываем результат преобразования(берем из набора 1-ый граф)
	strncpy_s(output, lengthOut, out.c_str(), lengthOut);

}

DLL_API void MemoryLeaks(wchar_t** s_array, int& s_len)
{
	//запись утечек в файл и парсинг оттуда в с# код
	_CrtSetDbgFlag(_CRTDBG_ALLOC_MEM_DF | _CRTDBG_LEAK_CHECK_DF);
	//short* ii = new short(92);
	CoTaskMemFree(*s_array);
	HANDLE hLogFile;
	hLogFile = CreateFile(L"log.txt", GENERIC_WRITE,
		FILE_SHARE_WRITE, NULL, CREATE_ALWAYS,
		FILE_ATTRIBUTE_NORMAL, NULL);
	_CrtSetReportMode(_CRT_WARN, _CRTDBG_MODE_FILE);
	_CrtSetReportFile(_CRT_WARN, hLogFile);
	_CrtDumpMemoryLeaks();
	CloseHandle(hLogFile);
	std::wifstream in(L"log.txt");
	std::wstring  ws((std::istreambuf_iterator<wchar_t >(in)), std::istreambuf_iterator<wchar_t>());
	wchar_t* n_sarr = (wchar_t*)CoTaskMemAlloc((ws.size() + 1) * sizeof(wchar_t*));
	ZeroMemory((n_sarr), (ws.size() + 1) * sizeof(wchar_t));
	StringCchCatW(n_sarr, ws.size() + 1, ws.data());

	*s_array = n_sarr;
	s_len = ws.size() + 1;
}
