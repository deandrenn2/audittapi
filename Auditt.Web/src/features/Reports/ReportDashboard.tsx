import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { useReportsGlobal } from "./UseReports";
import { faFileDownload } from "@fortawesome/free-solid-svg-icons";
import { useFileDownload } from "../../shared/components/FilesDowload";
import useUserContext from "../../shared/context/useUserContext";

export const ReportDashboard = ({ dataCut, idGuide }: { dataCut: number, idGuide: number }) => {
    const { reportGlobal } = useReportsGlobal(dataCut, idGuide);
    const { client } = useUserContext();
    const { descargarArchivo } = useFileDownload();

    const handleDownload = async () => {
        const urlBlob = `/api/reports/${dataCut}?idGuide=${idGuide}&idInstitution=${client?.id}`;
        await descargarArchivo(urlBlob, "ReportGeneral_" + new Date().toISOString().split('T')[0] + ".xlsx");
    }


    return (
        <div className="w-full">
            <div className="py-2">
                <div className="flex space-x-4 mb-4 justify-center">
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
                    <div className="flex flex-col gap-2 items-center bg-green-600 hover:bg-green-800 transition-all text-white p-8 rounded-4xl text-2xl cursor-pointer" onClick={() => handleDownload()}>
                        <span>Report</span>
                        <span className="text-2xl font-bold"><FontAwesomeIcon icon={faFileDownload} className="fa-2x" /></span>
                    </div>
                </div>
            </div>
        </div>
    )
}