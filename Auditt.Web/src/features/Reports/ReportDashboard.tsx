import { useReportsGlobal } from "./UseReports";

export const ReportDashboard = ({ dataCut, idGuide }: { dataCut: number, idGuide: number }) => {
    const { reportGlobal } = useReportsGlobal(dataCut, idGuide);

    if (!dataCut || !idGuide) {
        return <div></div>;
    }

    return (
        <div className="w-full">
            <div className="py-2">
                <div className="flex space-x-4 mb-4">
                    <div className="flex flex-col  items-center bg-audittpurple text-white p-8 rounded-4xl text-2xl">
                        <span>Adherencia Global</span>
                        <span className="text-4xl font-bold">{reportGlobal?.globalAdherence}%</span>
                    </div>
                    <div className="flex flex-col items-center  bg-audittpink text-audittblack p-8 rounded-4xl text-2xl">
                        <span>Adherencia Estricta</span>
                        <span className="text-4xl font-bold">{reportGlobal?.strictAdherence}%</span>
                    </div>
                    <div className="flex flex-col items-center  bg-audittprimary text-white p-8 rounded-4xl text-2xl">
                        <span>Historias</span>
                        <span className="text-4xl font-bold">{reportGlobal?.countHistories}</span>
                    </div>
                </div>
            </div>
        </div>
    )
}