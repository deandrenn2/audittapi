import React from "react"
import { ReportModel } from "../ReportModel"

export const ReportList = ({ listReports, idSelected, setSelected }: { listReports: ReportModel[], idSelected: number, setSelected: React.Dispatch<React.SetStateAction<number>> }) => {
    return (
        <div className="w-full">
            <div className=" py-2">
                <h1 className="text-2xl font-semibold mb-2">Lista de Reportes</h1>
                <div className="overflow-x-scroll ">
                    <div className="flex space-y-4 w-1/2 wrap-anywhere gap-4">
                        {listReports.map((report) => (
                            <div onClick={() => setSelected(report.idReport)} key={report.idReport} className={report.idReport == idSelected ?
                                "flex flex-col border-audittpinkgray border-2 p-4 rounded-4xl bg-pink-50 hover:bg-pink-50 transition-colors cursor-pointer" :
                                "flex flex-col border-audittpinkgray border-2 p-4 rounded-4xl  hover:bg-pink-50 transition-colors cursor-pointer"}>
                                <span className="font-semibold">{report.name}</span>
                                <span>{report.description}</span>
                            </div>
                        ))}
                    </div>
                </div>
            </div>
        </div>
    )
}