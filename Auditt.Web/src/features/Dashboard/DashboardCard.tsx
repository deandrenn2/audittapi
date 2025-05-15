export const DashboardCard = ({ title, value, textColor }: { title: string, value: number, textColor?: string }) => {
    return (
        <div className={`bg-audittpink-light text-audittpurple rounded-2xl p-8 border-audittprimary border-4 text-center`}>
            <p className=" font-bold text-4xl">{title}</p>
            <p className={`text-6xl font-bold mt-2 ${textColor ?? 'text-audittpurple'}`}>{value}</p>
        </div>
    );
};