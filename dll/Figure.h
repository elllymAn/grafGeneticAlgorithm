#pragma once
class Figure
{
public:
	Figure() {}
	virtual ~Figure() {}
	Figure(const Figure& obj) {}
	Figure& operator=(const Figure& obj) { return *this; }
	virtual void OutInfo(std::ofstream& file) = 0;
	virtual Figure* copy() = 0;
};