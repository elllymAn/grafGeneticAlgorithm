// dll.cpp : Defines the exported functions for the DLL.
//

#include "pch.h"
#include "framework.h"
#include "dll.h"


// This is an example of an exported variable
DLL_API void geneticModify(const char* input, char* output, char* richText, int lengthOut)
{
	//int* a = new int(5);

	//��������� ���������� � �������� ������� ������: ��� �������,��� �������,...,��� �������;��� �������-��� �������,...,��� �������-��� �������(�����)
	std::string* in = new std::string(input);
	//������� �� ������ ������ ����
	Graph* g = new Graph(*in);
	//����������� ���� �� ������������� ��������� � �������� ������ �� ���� ����� � ���� ������. ��������� ����� ���� ��������� ������
	std::vector <Graph> v = (g->geneticAlgorithm());
	// ���������� ��������� ����� � DB
	DB* db = new DB(v);
	std::string out = v[0].codeGraf();
	// ���������� ���������� � ����
	std::ofstream file("logGraf.txt");
	db->returnLog(file);
	file.close();
	std::ifstream fileR("logGraf.txt");
	std::stringstream buffer;
	buffer << fileR.rdbuf();
	fileR.close();
	std::string str = buffer.str();
	strncpy_s(richText, lengthOut, str.c_str(), lengthOut);
	//������� ������
	delete db;
	delete g;
	delete in;
	//���������� ��������� ��������������(����� �� ������ 1-�� ����)
	strncpy_s(output, lengthOut, out.c_str(), lengthOut);

}

DLL_API void MemoryLeaks(wchar_t** s_array, int& s_len)
{
	//������ ������ � ���� � ������� ������ � �# ���
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
