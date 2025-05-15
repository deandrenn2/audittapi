import { useReportsQuestionAdherence } from "./UseReports";

export const ReportGeneral = ({ dataCut, idGuide }: { dataCut: number, idGuide: number }) => {
    const { reportQuestionAdherence } = useReportsQuestionAdherence(dataCut, idGuide);

    if (!dataCut || !idGuide) {
        return <div></div>;
    }
    return (
        <div className="w-full">
            <div className="flex py-2">
                <div className="flex flex-col space-x-4 mb-4">
                    {reportQuestionAdherence?.map((valuation, index) => (
                        <div key={index} className="w-full mb-1 bg-green-100 border-2 border-green-200 rounded-2xl p-4 gap-2 flex justify-between items-center">
                            <h2>{valuation.text}</h2>
                            <h2 className="text-2xl font-bold text-audittpurple">{valuation.percentSuccess}%</h2>
                        </div>
                    ))}
                </div>
            </div>
        </div>
    )
}