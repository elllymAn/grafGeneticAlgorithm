#pragma once
class Graph
{
private:
	std::vector <Edge*>* arrEdge;
	std::set <Point*>* arrPoint;
	std::multimap <int, std::vector <Point*>> GetRating();
	int getWeightOfCase(std::vector <Point*>& Case);
public:
	Graph(std::string data);
	Graph(const Graph& obj);
	std::string geneticAlgorithm();
	Graph& operator=(const Graph& obj) = delete;
	~Graph();
};

