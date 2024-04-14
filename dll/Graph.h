#pragma once
class Graph
{
private:
	std::vector <Figure*>* dataOut;
	std::vector <Edge*>* arrEdge;
	std::set <Point*>* arrPoint;
	std::multimap <int, std::vector <Point*>> GetRating();
	int getWeightOfCase(std::vector <Point*>& Case);
public:
	std::vector<Figure*>* outInfo() { return dataOut; }
	Graph(std::string data);
	Graph(const Graph& obj);
	std::vector <Graph> geneticAlgorithm();
	Graph& operator=(const Graph& obj) = delete;
	bool operator<(const Graph& obj);
	std::string codeGraf();
	~Graph();
};

